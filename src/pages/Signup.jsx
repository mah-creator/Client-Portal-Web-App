import { useState } from "react";

export default function Signup() {
  const [form, setForm] = useState({
    name: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const [success, setSuccess] = useState(false);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (form.password !== form.confirmPassword) {
      alert("Passwords do not match");
      return;
    }

    // 👉 Replace with real signup API call
    console.log("User signed up:", form);

    // Show success screen
    setSuccess(true);
  };

  if (success) {
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
              <span className="font-medium text-slate-800">{form.email}</span>.
              Please check your inbox to verify your account.
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

  return (
    <div className="flex justify-center items-center bg-gradient-to-b from-sky-100 via-sky-50 to-sky-100 p-4 w-full min-h-screen">
      <div className="w-full max-w-md">
        <div className="bg-white/90 shadow-xl backdrop-blur-sm mx-auto p-8 rounded-3xl ring-1 ring-black/5">
          <h1 className="font-semibold text-slate-800 text-3xl text-center tracking-tight">
            Create Account
          </h1>
          <p className="mt-2 text-slate-500 text-center">
            Join our platform and get started
          </p>

          <form className="space-y-4 mt-8" onSubmit={handleSubmit}>
            <div>
              <label className="block font-medium text-slate-700 text-sm">
                Name
              </label>
              <input
                type="text"
                name="name"
                placeholder="Your name"
                className="shadow-sm mt-1 px-4 py-2.5 border border-slate-200 focus:border-blue-500 rounded-xl focus:ring-4 focus:ring-blue-200 w-full"
                value={form.name}
                onChange={handleChange}
                required
              />
            </div>

            <div>
              <label className="block font-medium text-slate-700 text-sm">
                Email
              </label>
              <input
                type="email"
                name="email"
                placeholder="Enter your email"
                className="shadow-sm mt-1 px-4 py-2.5 border border-slate-200 focus:border-blue-500 rounded-xl focus:ring-4 focus:ring-blue-200 w-full"
                value={form.email}
                onChange={handleChange}
                required
              />
            </div>

            <div>
              <label className="block font-medium text-slate-700 text-sm">
                Password
              </label>
              <input
                type="password"
                name="password"
                placeholder="Enter your password"
                className="shadow-sm mt-1 px-4 py-2.5 border border-slate-200 focus:border-blue-500 rounded-xl focus:ring-4 focus:ring-blue-200 w-full"
                value={form.password}
                onChange={handleChange}
                required
              />
            </div>

            <div>
              <label className="block font-medium text-slate-700 text-sm">
                Confirm Password
              </label>
              <input
                type="password"
                name="confirmPassword"
                placeholder="Confirm your password"
                className="shadow-sm mt-1 px-4 py-2.5 border border-slate-200 focus:border-blue-500 rounded-xl focus:ring-4 focus:ring-blue-200 w-full"
                value={form.confirmPassword}
                onChange={handleChange}
                required
              />
            </div>

            <button
              type="submit"
              className="bg-blue-600 hover:bg-blue-700 shadow-sm mt-2 px-4 py-2.5 rounded-xl focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-blue-300 w-full text-white transition"
            >
              Sign Up
            </button>

            <p className="mt-4 text-slate-600 text-sm text-center">
              Already have an account?{" "}
              <a
                href="/login"
                className="font-medium text-blue-600 hover:text-blue-700"
              >
                Sign in
              </a>
            </p>
          </form>
        </div>
      </div>
    </div>
  );
}
