import ClockIcon from "../../icons/ClockIcon";

function NotificationItem({ title, project, time }) {
  return (
    <div className="flex items-start gap-3 hover:bg-slate-50 p-3 rounded-xl">
      <span className="bg-blue-500 mt-2 rounded-full w-2 h-2" />
      <div>
        <p className="font-medium text-sm leading-snug">{title}</p>
        <div className="flex items-center gap-3 mt-1 text-slate-500 text-xs">
          <span>{project}</span>
          <span className="inline-flex items-center gap-1">
            <ClockIcon />
            <span>{time}</span>
          </span>
        </div>
      </div>
    </div>
  );
}

export default NotificationItem;
