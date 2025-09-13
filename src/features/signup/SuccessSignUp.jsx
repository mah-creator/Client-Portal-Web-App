function SuccessSignUp({ email }) {
  return (
    <div className="flex justify-center items-center bg-gradient-to-b from-sky-100 via-sky-50 to-sky-100 p-4 w-full min-h-screen">
      <div className="w-full max-w-md">
        <div className="bg-white/90 shadow-xl backdrop-blur-sm mx-auto p-8 rounded-3xl ring-1 ring-black/5 text-center">
          <div className="flex justify-center items-center bg-green-500 shadow-sm mx-auto mb-6 rounded-full w-16 h-16 text-white">
            <svg
              viewBox="0 0 24 24"
              className="w-8 h-8"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
            >
              <path d="M20 6L9 17l-5-5" />
            </svg>
          </div>
          <h1 className="font-semibold text-slate-800 text-2xl">
            Successfully Signed Up!
          </h1>
          <p className="mt-2 text-slate-600">
            We’ve sent a confirmation email to{" "}
            <span className="font-medium text-slate-800">{email}</span>. Please
            check your inbox to verify your account.
          </p>
          <a
            href="/login"
            className="inline-block bg-blue-600 hover:bg-blue-700 shadow-sm mt-6 px-6 py-2.5 rounded-xl focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-blue-300 text-white transition"
          >
            Go to Login
          </a>
        </div>
      </div>
    </div>
  );
}

export default SuccessSignUp;
