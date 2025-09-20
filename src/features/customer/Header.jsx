function Header({ data }) {
  return (
    <>
      <header className="mb-8">
        <h1 className="font-semibold text-2xl">Client Portal</h1>
        <p className="text-slate-500">Customer Dashboard</p>
      </header>
      <section className="mb-6">
        <h2 className="font-bold text-3xl tracking-tight">
          Welcome back, {data.name}
        </h2>
        <p className="mt-2 text-slate-600">
          Here's what's happening with your projects
        </p>
      </section>
    </>
  );
}

export default Header;
