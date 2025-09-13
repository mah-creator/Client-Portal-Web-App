function About() {
  return (
    <section id="about" className="mx-auto px-6 py-14 max-w-5xl">
      <div className="gap-8 grid md:grid-cols-2">
        <div>
          <h2 className="font-semibold text-2xl">About Client Portal</h2>
          <p className="mt-3 text-slate-600">
            Client Portal streamlines collaboration between service providers
            and their clients. Share milestones, files, and progress updates
            with role-based access (Freelancer & Customer), while your admin
            tools stay private.
          </p>
          <ul className="mt-4 pl-5 text-slate-600 list-disc">
            <li>Tasks, milestones, and progress tracking</li>
            <li>File sharing with comments and approvals</li>
            <li>Simple, secure access for your clients</li>
          </ul>
        </div>
        <div className="bg-white shadow p-6 border border-slate-200 rounded-2xl">
          <h3 className="font-medium">Why teams choose us</h3>
          <div className="gap-4 grid sm:grid-cols-2 mt-4">
            <Feature
              icon="📦"
              title="Project hubs"
              text="Keep scope, files, and discussion in one place."
            />
            <Feature
              icon="🔒"
              title="Secure by default"
              text="Role-based access; your admin tools stay hidden."
            />
            <Feature
              icon="⏱️"
              title="Fast onboarding"
              text="Invite clients in minutes—no learning curve."
            />
            <Feature
              icon="✅"
              title="Clear delivery"
              text="Show progress and approvals without back-and-forth."
            />
          </div>
        </div>
      </div>
    </section>
  );
}

export default About;

function Feature({ icon, title, text }) {
  return (
    <div className="flex items-start gap-3 bg-white shadow-sm p-4 border border-slate-200 rounded-xl">
      <div className="place-items-center grid bg-blue-50 rounded-lg w-9 h-9 text-blue-600">
        {icon}
      </div>
      <div>
        <p className="font-medium">{title}</p>
        <p className="text-slate-600 text-sm">{text}</p>
      </div>
    </div>
  );
}
