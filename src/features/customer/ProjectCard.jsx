import CalendarIcon from "../../icons/CalendarIcon";
import ClockIcon from "../../icons/ClockIcon";
import UsersIcon from "../../icons/UsersIcon";

function ProjectCard({
  title,
  owner,
  due,
  status,
  tasksDone,
  tasksTotal,
  progress,
  updated,
}) {
  return (
    <article className="bg-white shadow-sm mb-4 p-5 border border-slate-200 rounded-2xl">
      <div className="flex justify-between items-start gap-3">
        <div>
          <h4 className="font-semibold text-lg">{title}</h4>
          <div className="flex flex-wrap items-center gap-4 mt-2 text-slate-600 text-sm">
            <span className="inline-flex items-center gap-2">
              <UsersIcon />
              {owner}
            </span>
            <span className="inline-flex items-center gap-2">
              <CalendarIcon />
              Due {due}
            </span>
          </div>
        </div>

        <div className="flex items-center gap-4">
          <span className="text-slate-500 text-sm">
            {tasksDone}/{tasksTotal} tasks completed
          </span>
          {status?.label && (
            <span
              className={`rounded-full px-3 py-1 text-xs font-medium ${status.color}`}
            >
              {status.label}
            </span>
          )}
        </div>
      </div>

      {/* Progress */}
      <div className="bg-slate-100 mt-4 rounded-full">
        <div
          className="bg-blue-200 rounded-full h-2"
          style={{ width: `${Math.min(Math.max(progress, 0), 100)}%` }}
        />
      </div>

      <div className="flex justify-end items-center gap-2 mt-2 text-slate-500 text-sm">
        <ClockIcon />
        Updated {updated}
      </div>
    </article>
  );
}

export default ProjectCard;
