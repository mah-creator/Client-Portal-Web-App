import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';
import { Notification } from '@/types/api';
import { toast } from '@/hooks/use-toast';
import { useEffect } from 'react';
import { signalRService } from '@/lib/signalr';
import { useLocation } from 'react-router-dom';
import { getNotificationUrl } from '@/lib/utils';

export const useNotifications = (page: number = 1, pageSize: number = 10) => {
  const queryClient = useQueryClient();
  const location = useLocation();

  const { data: notificationsData, isLoading } = useQuery({
    queryKey: ['notifications', page, pageSize],
    queryFn: () => apiClient.getNotifications({ page, pageSize }),
  });

  const notifications = notificationsData?.items || [];
  const totalPages = notificationsData ? Math.ceil(notificationsData.totalCount / pageSize) : 0;

  const markAsReadMutation = useMutation({
    mutationFn: (id: string) => apiClient.markNotificationAsRead(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notifications'] });
    },
    onError: (error: Error) => {
      toast({
        title: 'Error',
        description: error.message,
        variant: 'destructive',
      });
    },
  });

  const markAllAsReadMutation = useMutation({
    mutationFn: () => apiClient.markAllNotificationAsRead(),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notifications'] });
    },
    onError: (error: Error) => {
      toast({
        title: 'Error',
        description: error.message,
        variant: 'destructive',
      });
    },
  });

  // Connect to SignalR and listen for real-time notifications
  useEffect(() => {
    const token = apiClient.getToken();
    if (!token) return;

    const connectSignalR = async () => {
      try {
        if (!signalRService.isConnected()) {
          await signalRService.connect(token);
        }

        const unsubscribe = signalRService.onNotification((notification) => {
          // Invalidate all notification queries to refetch
          queryClient.invalidateQueries({ queryKey: ['notifications'] });

          // Check if user is on the target page and auto-refetch relevant data
          const targetUrl = notification.metadata 
            ? getNotificationUrl(notification.metadata)
            : notification.actionUrl || '/';
          
          const isOnTargetPage = location.pathname === targetUrl || location.pathname + location.search === targetUrl;
          
          // If on target page, invalidate relevant queries to refetch data
          if (isOnTargetPage && notification.metadata) {
            const { resourceType, projectId, taskId } = notification.metadata;
            
            if (resourceType === 'project' && projectId) {
              queryClient.invalidateQueries({ queryKey: ['projects'] });
              queryClient.invalidateQueries({ queryKey: ['project', projectId] });
            } else if (resourceType === 'task' && projectId) {
              queryClient.invalidateQueries({ queryKey: ['tasks', projectId] });
              if (taskId) {
                queryClient.invalidateQueries({ queryKey: ['task', taskId] });
              }
            } else if (resourceType === 'file' && projectId) {
              queryClient.invalidateQueries({ queryKey: ['files', projectId] });
            } else if (resourceType === 'invitation') {
              queryClient.invalidateQueries({ queryKey: ['invitations'] });
            }
          }

          // Show toast for new notification
          if (notification.type != 'new_comment')
          {
            toast({
              title: notification.title,
              description: notification.message,
            });
          }
          });

        return unsubscribe;
      } catch (error) {
        console.error('Failed to connect to SignalR:', error);
      }
    };

    const cleanupPromise = connectSignalR();

    return () => {
      cleanupPromise.then(cleanup => cleanup?.());
    };
  }, [queryClient, location]); // Depend on queryClient and location

  const unreadCount = notifications.filter(n => !n.isRead).length;

  return {
    notifications,
    unreadCount,
    isLoading,
    totalPages,
    totalCount: notificationsData?.totalCount || 0,
    hasMore: notificationsData ? (page * pageSize) < notificationsData.totalCount : false,
    markAsRead: markAsReadMutation.mutate,
    markAllAsRead: markAllAsReadMutation.mutate,
    isMarkingAsRead: markAsReadMutation.isPending,
  };
};