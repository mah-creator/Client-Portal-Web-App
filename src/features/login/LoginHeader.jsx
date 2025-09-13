function LoginHeader() {
  return (
    <>
      <div className="flex justify-center items-center bg-gradient-to-b from-blue-500 to-blue-600 shadow-sm mx-auto mb-6 rounded-2xl w-16 h-16 text-white">
        <svg
          viewBox="0 0 24 24"
          className="w-8 h-8"
          fill="none"
          stroke="currentColor"
          strokeWidth="1.8"
        >
          <rect x="5" y="7" width="14" height="12" rx="2" />
          <path d="M9 7V5a2 2 0 0 1 2-2h2a2 2 0 0 1 2 2v2" />
          <path d="M9 11h6M9 15h6" />
        </svg>
      </div>

      <h1 className="font-semibold text-slate-800 text-3xl text-center tracking-tight">
        Client Portal
      </h1>
      <p className="mt-2 text-slate-500 text-center">
        Professional project management platform
      </p>
    </>
  );
}

export default LoginHeader;
