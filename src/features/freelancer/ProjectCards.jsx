import ActivityItem from "./ActivityItem";
import ProjectCard from "./ProjectCard";

function ProjectCards({ projects, activity }) {
  return (
    <section className="items-start gap-6 grid lg:grid-cols-3 mt-10">
      <div className="lg:col-span-2">
        <h2 className="mb-4 font-semibold text-xl">Active Projects</h2>
        {projects.map((p) => (
          <ProjectCard key={p.id} {...p} />
        ))}
      </div>
      <aside>
        <h2 className="mb-4 font-semibold text-xl">Recent Activity</h2>
        <div className="bg-white shadow p-4 border border-slate-200 rounded-2xl">
          {activity.map((a) => (
            <ActivityItem key={a.id} text={a.text} time={a.time} />
          ))}
        </div>
      </aside>
    </section>
  );
}

export default ProjectCards;
