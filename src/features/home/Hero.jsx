function Hero() {
  return (
    <section className="mx-auto px-6 pt-6 md:pt-10 pb-10 max-w-6xl">
      <div className="items-center gap-10 grid md:grid-cols-2">
        <div>
          <h1 className="font-bold text-4xl md:text-5xl leading-tight">
            Professional project management for teams and clients
          </h1>
          <p className="mt-4 text-slate-600 text-lg">
            Track projects, share progress, and collaborate securely in one
            place—built for freelancers, agencies, and their customers.
          </p>
          <div className="flex flex-wrap items-center gap-3 mt-6">
            <a
              href="/login"
              className="bg-blue-600 hover:bg-blue-700 shadow-sm px-5 py-3 rounded-xl focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-blue-300 text-white transition"
            >
              Log in
            </a>
            <a
              href="#about"
              className="hover:bg-white hover:shadow-sm px-5 py-3 border border-slate-300 rounded-xl text-slate-700"
            >
              Learn more
            </a>
          </div>

          <div className="flex flex-wrap gap-6 mt-8 text-slate-600 text-sm">
            <div>
              <span className="block font-semibold text-slate-900 text-2xl">
                5,000+
              </span>
              Teams onboarded
            </div>
            <div>
              <span className="block font-semibold text-slate-900 text-2xl">
                98%
              </span>
              On-time delivery rate
            </div>
            <div>
              <span className="block font-semibold text-slate-900 text-2xl">
                24/7
              </span>
              Secure & available
            </div>
          </div>
        </div>
        <div className="relative">
          <div className="bg-white shadow-xl p-6 border border-slate-200 rounded-3xl">
            <div className="bg-gradient-to-br from-blue-50 to-slate-50 rounded-2xl ring-1 ring-slate-100 w-full aspect-[4/3]" />
            <p className="mt-3 text-slate-500 text-sm text-center">
              Preview of your client workspace
            </p>
          </div>
          <div className="hidden md:block -top-4 -right-4 absolute bg-blue-600 shadow px-3 py-1 rounded-xl text-white text-xs">
            New: Task comments
          </div>
        </div>
      </div>
    </section>
  );
}

export default Hero;
