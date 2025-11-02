import { useState, useContext, useEffect } from "react";
import { useSearchParams } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { AuthContext } from "../App";
import { useProjects } from "@/hooks/useProjects";
import { CreateProjectDialog } from "@/components/CreateProjectDialog";
import { 
  Plus, 
  Calendar, 
  Clock, 
  Users, 
  FileText, 
  Bell,
  LogOut,
  BarChart3,
  CheckCircle,
  AlertCircle,
  AlertTriangle,
  TrendingUp,
  UserCircle,
  ArrowUpRight,
  PlayCircle,
  Eye,
  CheckCircle2,
  CircleX,
  DollarSign
} from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useCompletedTasks, usePendingTasks } from "@/hooks/useTasks";
import { useProfile } from "@/hooks/useProfile";
import { API_BASE_URL, apiClient } from "@/lib/api-client";
import { cn } from "@/lib/utils";
import { NotificationsDropdown } from "@/components/NotificationsDropdown";
import { Pagination } from "@/components/ui/Pagination";
import { UserStats } from "@/types/api";

const FreelancerDashboard = () => {
  const { profile } = useProfile();
  const [avatarTimestamp] = useState(Date.now());
  const { user, logout } = useContext(AuthContext);
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [projectsPage, setProjectsPage] = useState(1);
  const [selectedStatus, setSelectedStatus] = useState<string>('all');
  const [highlightedProjectId, setHighlightedProjectId] = useState<string | null>(null);
  const projectsPageSize = 3;
  const { projects, isLoading, totalPages } = useProjects(
    projectsPage, 
    projectsPageSize,
    selectedStatus === 'all' ? undefined : selectedStatus
  );
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);

  // Calculate stats from real data
  const activeProjects = projects.filter(p => p.status === 'Active').length;
  const completedTasks = useCompletedTasks()?.task?.length ?? 0;
  const pendingTasks = usePendingTasks()?.task?.length ?? 0;

  const [stats, setStats] = useState<UserStats | null>(null);

  const recentActivity = [
    { type: "task", message: "Task 'Homepage Design' marked as completed", time: "2 hours ago" },
    { type: "comment", message: "New comment from TechCorp Inc.", time: "4 hours ago" },
    { type: "file", message: "Design assets uploaded to Mobile App project", time: "6 hours ago" },
    { type: "project", message: "New project invitation sent to Creative Agency", time: "1 day ago" }
  ];

  useEffect(() => {
      if (user) {
        fetchStats();
      }
    }, [user]);

  const fetchStats = async () => {
      try {
          const data = await apiClient.getUserStats();
          setStats(data);
      } catch (error) {
        console.error('Failed to fetch stats:', error);
      }
    };

  const getStatusBadge = (status: string) => {
    const variants: Record<string, string> = {
      'Deleted': 'bg-red-100 text-red-800 border-red-200',
      'Active': 'bg-blue-100 text-blue-800 border-blue-200',
      'Completed': 'bg-green-100 text-green-800 border-green-200',
      'Pending_Complete_Approval': 'bg-amber-100 text-amber-800 border-amber-200',
      'Pending_Delete_Approval': 'bg-orange-100 text-orange-800 border-orange-200'
    };
    return variants[status] || 'bg-gray-100 text-gray-800 border-gray-200';
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'Active': return <PlayCircle className="w-4 h-4 text-blue-600" />;
      case 'Completed': return <CheckCircle2 className="w-4 h-4 text-green-600" />;
      case 'Pending_Complete_Approval': return <Clock className="w-4 h-4 text-amber-600" />;
      case 'Pending_Delete_Approval': return <AlertTriangle className="w-4 h-4 text-orange-600" />;
      case 'Deleted': return <CircleX className="w-4 h-4 text-red-600" />;
      default: return <FileText className="w-4 h-4 text-gray-600" />;
    }
  };

  // Handle deep linking to projects
  useEffect(() => {
    const projectId = searchParams.get('projectId');
    if (projectId) {
      setHighlightedProjectId(projectId);
      
      // Scroll to project
      setTimeout(() => {
        const projectElement = document.getElementById(`project-${projectId}`);
        if (projectElement) {
          projectElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
      }, 100);

      // Clear highlight and URL after animation
      setTimeout(() => {
        setHighlightedProjectId(null);
        searchParams.delete('projectId');
        setSearchParams(searchParams, { replace: true });
      }, 3000);
    }
  }, [searchParams, setSearchParams]);

  return (
    <div className="min-h-screen bg-gradient-subtle">
      {/* Header */}
      <header className="border-b bg-white/50 backdrop-blur-sm sticky top-0 z-10">
        <div className="max-w-[var(--content-width)] mx-auto px-6 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-4">
              <div className="w-10 h-10 bg-gradient-to-br from-blue-600 to-blue-700 rounded-xl flex items-center justify-center shadow-sm">
                <FileText className="w-5 h-5 text-white" />
              </div>
              <div>
                <h1 className="text-xl font-bold text-gray-900">Client Portal</h1>
                <p className="text-sm text-gray-600">Freelancer Dashboard</p>
              </div>
            </div>
            
            <div className="flex items-center gap-4">
              <NotificationsDropdown />
              
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button variant="ghost" className="flex items-center gap-2 hover:bg-gray-100 transition-colors">
                    <Avatar className="w-8 h-8 border border-gray-200">
                      <AvatarImage 
                        src={profile?.avatarUrl ? `${API_BASE_URL}${profile.avatarUrl}?t=${avatarTimestamp}` : ''} 
                      />
                      <AvatarFallback className="bg-blue-100 text-blue-600 text-sm">
                        {user?.name.split(' ').map(n => n[0]).join('')}
                      </AvatarFallback>
                    </Avatar>
                    <div className="text-sm text-left">
                      <div className="font-medium text-gray-900">{profile?.name}</div>
                      <div className="text-gray-600 capitalize">{user?.role}</div>
                    </div>
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="w-56 border border-gray-200 shadow-lg">
                  <DropdownMenuLabel className="text-gray-900">My Account</DropdownMenuLabel>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem 
                    onClick={() => navigate('/profile')}
                    className="cursor-pointer text-gray-700 hover:bg-gray-50"
                  >
                    <UserCircle className="w-4 h-4 mr-2 text-gray-600" />
                    Profile Settings
                  </DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem 
                    onClick={logout}
                    className="cursor-pointer text-red-600 hover:bg-red-50"
                  >
                    <LogOut className="w-4 h-4 mr-2" />
                    Log out
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </div>
          </div>
        </div>
      </header>

      <div className="max-w-[var(--content-width)] mx-auto px-6 py-8">
        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          {/* Active Projects Card */}
          <Card className="bg-gradient-to-br from-white to-blue-50 border border-blue-100 shadow-sm hover:shadow-lg transition-all duration-300 hover:-translate-y-1">
            <CardContent className="p-6">
              <div className="flex items-start justify-between">
                <div className="space-y-2">
                  <p className="text-sm font-semibold text-blue-700 uppercase tracking-wide">Active Projects</p>
                  <p className="text-3xl font-bold text-gray-900">{stats?.projectsCount || 0}</p>
                  <div className="flex items-center">
                    <div className="w-2 h-2 bg-blue-500 rounded-full mr-2"></div>
                    <p className="text-xs text-gray-600">Currently in progress</p>
                  </div>
                </div>
                <div className="w-14 h-14 bg-gradient-to-br from-blue-500 to-blue-600 rounded-2xl flex items-center justify-center shadow-lg">
                  <BarChart3 className="w-7 h-7 text-white" />
                </div>
              </div>
            </CardContent>
          </Card>
          
          {/* Tasks Completed Card */}
          <Card className="bg-gradient-to-br from-white to-green-50 border border-green-100 shadow-sm hover:shadow-lg transition-all duration-300 hover:-translate-y-1">
            <CardContent className="p-6">
              <div className="flex items-start justify-between">
                <div className="space-y-2">
                  <p className="text-sm font-semibold text-green-700 uppercase tracking-wide">Tasks Completed</p>
                  <p className="text-3xl font-bold text-gray-900">{stats?.tasksCompleted || 0}</p>
                  <div className="flex items-center">
                    <TrendingUp className="w-3 h-3 text-green-500 mr-2" />
                    <p className="text-xs text-gray-600">
                      All time
                    </p>
                  </div>
                </div>
                <div className="w-14 h-14 bg-gradient-to-br from-green-500 to-green-600 rounded-2xl flex items-center justify-center shadow-lg">
                  <CheckCircle className="w-7 h-7 text-white" />
                </div>
              </div>
            </CardContent>
          </Card>
          
          {/* Pending Tasks Card */}
          <Card className="bg-gradient-to-br from-white to-amber-50 border border-amber-100 shadow-sm hover:shadow-lg transition-all duration-300 hover:-translate-y-1">
            <CardContent className="p-6">
              <div className="flex items-start justify-between">
                <div className="space-y-2">
                  <p className="text-sm font-semibold text-amber-700 uppercase tracking-wide">Pending Tasks</p>
                  <p className="text-3xl font-bold text-gray-900">{stats?.tasksPending || 0}</p>
                  <div className="flex items-center">
                    <Clock className="w-3 h-3 text-amber-500 mr-2" />
                    <p className="text-xs text-gray-600">Requires attention</p>
                  </div>
                </div>
                <div className="w-14 h-14 bg-gradient-to-br from-amber-500 to-amber-600 rounded-2xl flex items-center justify-center shadow-lg">
                  <AlertCircle className="w-7 h-7 text-white" />
                </div>
              </div>
            </CardContent>
          </Card>
          
          {/* Revenue Card */}
          <Card className="bg-gradient-to-br from-white to-purple-50 border border-purple-100 shadow-sm hover:shadow-lg transition-all duration-300 hover:-translate-y-1">
            <CardContent className="p-6">
              <div className="flex items-start justify-between">
                <div className="space-y-2">
                  <p className="text-sm font-semibold text-purple-700 uppercase tracking-wide">Revenue</p>
                  <p className="text-3xl font-bold text-gray-900">
                    ${stats?.revenue}
                  </p>
                  <div className="flex items-center">
                    <DollarSign className="w-3 h-3 text-purple-500 mr-2" />
                    <p className="text-xs text-gray-600">Total revenue</p>
                  </div>
                </div>
                <div className="w-14 h-14 bg-gradient-to-br from-purple-500 to-purple-600 rounded-2xl flex items-center justify-center shadow-lg">
                  <TrendingUp className="w-7 h-7 text-white" />
                </div>
              </div>
            </CardContent>
          </Card>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-12">
          {/* Projects */}
          <div className="lg:col-start-2 lg:col-span-10">
            <div className="flex items-center justify-between mb-6">
              <div>
                <h2 className="text-xl font-semibold text-gray-900">Your Projects</h2>
                <p className="text-sm text-gray-600 mt-1">Manage your ongoing projects and tasks</p>
              </div>
              <Button 
                className="bg-blue-600 text-white hover:bg-blue-700 shadow-md transition-all duration-200"
                onClick={() => setIsCreateDialogOpen(true)}
              >
                <Plus className="w-4 h-4 mr-2" />
                New Project
              </Button>
            </div>

            {/* Status Filter Tabs */}
            <Tabs 
              value={selectedStatus} 
              onValueChange={(value) => {
                setSelectedStatus(value);
                setProjectsPage(1); // Reset to first page when changing status
              }}
              className="mb-6"
            >
              <TabsList className="grid w-full grid-cols-4 bg-gray-100/50">
                <TabsTrigger 
                  value="all" 
                  className="data-[state=active]:bg-white data-[state=active]:text-gray-900"
                >
                  All Projects
                </TabsTrigger>
                <TabsTrigger 
                  value="Active" 
                  className="data-[state=active]:bg-blue-100 data-[state=active]:text-blue-800"
                >
                  <PlayCircle className="w-4 h-4 mr-2" />
                  Active
                </TabsTrigger>
                <TabsTrigger 
                  value="Completed" 
                  className="data-[state=active]:bg-green-100 data-[state=active]:text-green-800"
                >
                  <CheckCircle2 className="w-4 h-4 mr-2" />
                  Completed
                </TabsTrigger>
                <TabsTrigger 
                  value="Deleted" 
                  className="data-[state=active]:bg-red-100 data-[state=active]:text-red-800"
                >
                  <CircleX className="w-4 h-4 mr-2" />
                  Deleted
                </TabsTrigger>
              </TabsList>
            </Tabs>
            
            <div className="space-y-4">
              {isLoading ? (
                <div className="space-y-4">
                  {[1, 2, 3].map((i) => (
                    <Card key={i} className="border border-gray-200 shadow-sm animate-pulse">
                      <CardContent className="p-6">
                        <div className="h-5 bg-gray-200 rounded w-3/4 mb-4"></div>
                        <div className="h-3 bg-gray-200 rounded w-1/2 mb-2"></div>
                        <div className="h-2 bg-gray-200 rounded w-full mb-4"></div>
                        <div className="h-2 bg-gray-200 rounded w-full"></div>
                      </CardContent>
                    </Card>
                  ))}
                </div>
              ) : projects.length === 0 ? (
                <Card className="border border-gray-200 shadow-sm text-center py-12">
                  <CardContent>
                    <div className="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
                      <FileText className="w-8 h-8 text-gray-400" />
                    </div>
                    <h3 className="font-semibold text-gray-900 mb-2">No projects yet</h3>
                    <p className="text-gray-600 mb-4">Create your first project to get started!</p>
                    <Button 
                      onClick={() => setIsCreateDialogOpen(true)}
                      className="bg-blue-600 text-white hover:bg-blue-700"
                    >
                      <Plus className="w-4 h-4 mr-2" />
                      Create Project
                    </Button>
                  </CardContent>
                </Card>
              ) : (
                projects.map((project) => {
                  const progress = project.progress || 0;
                  const isOverdue = project.dueDate && new Date(project.dueDate) < new Date();
                  
                  return (
                    <Card 
                      key={project.id}
                      id={`project-${project.id}`}
                      className={cn(
                        "border border-gray-200 shadow-sm hover:shadow-md transition-all duration-200 cursor-pointer group",
                        highlightedProjectId === project.id && "flash-highlight"
                      )}
                      onClick={() => navigate(`/project/${project.id}`)}
                    >
                      <CardContent className="p-6">
                        <div className="flex items-start justify-between mb-4">
                          <div className="flex items-start gap-3">
                            <div className="w-10 h-10 bg-blue-100 rounded-lg flex items-center justify-center mt-1">
                              {getStatusIcon(project.status)}
                            </div>
                            <div>
                              <h3 className="font-semibold text-gray-900 text-lg mb-1 group-hover:text-blue-600 transition-colors">
                                {project.title}
                              </h3>
                              <p className="text-gray-600 flex items-center gap-2 text-sm">
                                <Users className="w-4 h-4" />
                                {project.client || 'No client assigned'}
                              </p>
                            </div>
                          </div>
                          <Badge className={cn("border px-3 py-1 rounded-full text-sm font-medium", getStatusBadge(project.status))}>
                            {project.status.split('_').map((word, i) => 
                              i === 0 ? word.charAt(0).toUpperCase() + word.slice(1) : word
                            ).join(' ')}
                          </Badge>
                        </div>
                        
                        <div className="flex items-center justify-between text-sm text-gray-500 mb-4">
                          <span className="flex items-center gap-2">
                            <Calendar className="w-4 h-4" />
                            {project.dueDate ? (
                              <span className={isOverdue ? 'text-red-600 font-medium' : ''}>
                                Due {new Date(project.dueDate).toLocaleDateString()}
                              </span>
                            ) : (
                              'No due date'
                            )}
                          </span>
                          <span className="font-medium text-gray-700">
                            {project.tasksCompleted || 0}/{project.tasksTotal || 0} tasks
                          </span>
                        </div>
                        
                        <div className="space-y-2">
                          <div className="w-full bg-gray-200 rounded-full h-2">
                            <div 
                              className="bg-blue-500 h-2 rounded-full transition-all duration-300"
                              style={{ width: `${progress}%` }}
                            ></div>
                          </div>
                          <div className="flex items-center justify-between text-sm">
                            <span className="text-blue-600 font-medium">{progress.toFixed(1)}% complete</span>
                            <span className="text-gray-500 flex items-center gap-1 group-hover:text-blue-600 transition-colors">
                              View project
                              <ArrowUpRight className="w-3 h-3" />
                            </span>
                          </div>
                        </div>
                      </CardContent>
                    </Card>
                  );
                })
              )}
            </div>
            {totalPages > 1 && (
              <Pagination 
                currentPage={projectsPage}
                totalPages={totalPages}
                onPageChange={setProjectsPage}
                className="mt-6"
              />
            )}
          </div>

          {/* Activity Feed */}
          
          {/* <div> */}
            {/* <div className="mb-6">
              <h2 className="text-xl font-semibold text-gray-900">Recent Activity</h2>
              <p className="text-sm text-gray-600 mt-1">Latest updates across your projects</p>
            </div> */}
            {/* <Card className="border border-gray-200 shadow-sm">
              <CardContent className="p-6">
                <div className="space-y-4">
                  {recentActivity.map((activity, index) => (
                    <div key={index} className="flex gap-3 group">
                      <div className="flex flex-col items-center">
                        <div className={cn(
                          "w-2 h-2 rounded-full mt-2",
                          activity.type === 'task' ? 'bg-green-500' :
                          activity.type === 'comment' ? 'bg-blue-500' :
                          activity.type === 'file' ? 'bg-amber-500' : 'bg-purple-500'
                        )}></div>
                        {index < recentActivity.length - 1 && (
                          <div className="w-0.5 h-8 bg-gray-200 mt-1"></div>
                        )}
                      </div>
                      <div className="flex-1 pb-4 group-last:pb-0">
                        <p className="text-sm text-gray-700 group-hover:text-gray-900 transition-colors">
                          {activity.message}
                        </p>
                        <p className="text-xs text-gray-500 flex items-center gap-1 mt-1">
                          <Clock className="w-3 h-3" />
                          {activity.time}
                        </p>
                      </div>
                    </div>
                  ))}
                </div>
                
                <Button 
                  variant="ghost" 
                  size="sm" 
                  className="w-full mt-4 text-gray-600 hover:text-gray-900 hover:bg-gray-50"
                >
                  View all activity
                </Button>
              </CardContent>
            </Card> */}

            {/* Quick Stats */}
            {/* <Card className="border border-gray-200 shadow-sm mt-6">
              <CardHeader className="pb-4">
                <CardTitle className="text-lg font-semibold text-gray-900">Performance</CardTitle>
                <CardDescription className="text-gray-600">Your project completion rate</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-gray-600">Completion Rate</span>
                    <span className="text-lg font-bold text-green-600">92%</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-gray-600">On Time Delivery</span>
                    <span className="text-lg font-bold text-blue-600">88%</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-gray-600">Client Satisfaction</span>
                    <span className="text-lg font-bold text-amber-600">4.8/5</span>
                  </div>
                </div>
              </CardContent>
            </Card> */}
          {/* </div> */}
        </div>
      </div>

      <CreateProjectDialog 
        open={isCreateDialogOpen} 
        onOpenChange={setIsCreateDialogOpen}
      />
    </div>
  );
};

export default FreelancerDashboard;