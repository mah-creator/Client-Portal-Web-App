function Nav() {
  return (
    <header className="flex justify-between items-center mx-auto px-6 py-5 max-w-6xl">
      <div className="flex items-center gap-3">
        <span className="inline-flex justify-center items-center bg-gradient-to-b from-blue-500 to-blue-600 shadow rounded-xl w-10 h-10 text-white">
          <svg
            viewBox="0 0 24 24"
            className="w-6 h-6"
            fill="none"
            stroke="currentColor"
            strokeWidth="1.8"
          >
            <rect x="5" y="7" width="14" height="12" rx="2" />
            <path d="M9 7V5a2 2 0 0 1 2-2h2a2 2 0 0 1 2 2v2" />
            <path d="M9 11h6M9 15h6" />
          </svg>
        </span>
        <span className="font-semibold text-lg">Client Portal</span>
      </div>

      <div className="hidden md:flex items-center gap-4">
        <a
          href="#about"
          className="text-slate-600 hover:text-slate-900 text-sm"
        >
          About
        </a>
        <a
          href="#features"
          className="text-slate-600 hover:text-slate-900 text-sm"
        >
          Features
        </a>
        <a
          href="/login"
          className="bg-blue-600 hover:bg-blue-700 shadow px-4 py-2 rounded-xl focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-blue-300 font-medium text-white text-sm"
        >
          Log in
        </a>
      </div>
    </header>
  );
}

export default Nav;
