import { useState } from "react";

function SignupForm({ form, setForm, setSuccess }) {
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

  return (
    <form className="space-y-4 mt-8" onSubmit={handleSubmit}>
      <div>
        <label className="block font-medium text-slate-700 text-sm">Name</label>
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
  );
}

export default SignupForm;
