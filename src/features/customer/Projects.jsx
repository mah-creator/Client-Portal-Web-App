import ProjectCard from "./ProjectCard";

function Projects({ data }) {
  return (
    <div>
      <h3 className="mb-4 font-semibold text-xl">Your Projects</h3>
      {data.projects.map((p) => (
        <ProjectCard key={p.id} {...p} />
      ))}
    </div>
  );
}

export default Projects;
