function Features() {
  return (
    <section id="features" className="mx-auto px-6 pb-16 max-w-6xl">
      <h2 className="font-semibold text-2xl text-center">
        Everything you need to deliver on time
      </h2>
      <div className="gap-6 grid sm:grid-cols-2 lg:grid-cols-3 mt-8">
        <Card
          title="Tasks & Kanban"
          text="Organize work, assign owners, and track status."
        />
        <Card
          title="Files & Comments"
          text="Attach deliverables and discuss changes inline."
        />
        <Card
          title="Milestones"
          text="Break work into phases with due dates and reviews."
        />
        <Card
          title="Client View"
          text="A clean read-only view for customers to follow progress."
        />
        <Card
          title="Notifications"
          text="Email alerts when tasks move or files are approved."
        />
        <Card
          title="Analytics"
          text="See throughput and on-time delivery at a glance."
        />
      </div>

      <div className="mt-10 text-center">
        <a
          href="/login"
          className="inline-flex justify-center items-center bg-blue-600 hover:bg-blue-700 shadow-sm px-6 py-3 rounded-xl focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-blue-300 font-medium text-white transition"
        >
          Log in to get started
        </a>
      </div>
    </section>
  );
}

export default Features;

function Card({ title, text }) {
  return (
    <div className="bg-white shadow-sm p-5 border border-slate-200 rounded-2xl">
      <p className="font-medium">{title}</p>
      <p className="mt-1 text-slate-600 text-sm">{text}</p>
    </div>
  );
}
