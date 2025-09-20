import { useState } from "react";
import { signUp } from "../../mocks/mockApi"; // ⬅️ adjust path

function SignupForm({ form, setForm, setSuccess }) {
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    // basic validations
    if (!form.role) {
      setError("Please choose your role (freelancer or customer).");
      return;
    }
    if (form.password !== form.confirmPassword) {
      setError("Passwords do not match.");
      return;
    }
    if (form.password.length < 6) {
      setError("Password must be at least 6 characters.");
      return;
    }

    try {
      setSubmitting(true);
      // 🔗 call mock API – creates user in the right collection by role
      await signUp({
        name: form.name.trim(),
        email: form.email.trim(),
        password: form.password,
        role: form.role,
      });
      // show the success screen (“We’ve sent a confirmation email …”)
      setSuccess(true);
    } catch (err) {
      setError(err.message || "Sign up failed.");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form className="space-y-4 mt-8" onSubmit={handleSubmit}>
      {error && (
        <div className="bg-rose-50 px-3 py-2 border border-rose-200 rounded-lg text-rose-700 text-sm">
          {error}
        </div>
      )}

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

      {/* Role selection (choose here, not at login) */}
      <label className="block font-medium text-slate-700 text-sm">I am a</label>
      <div className="gap-2 grid sm:grid-cols-2 mt-2">
        {["freelancer", "customer"].map((r) => (
          <label
            key={r}
            className={
              "flex cursor-pointer items-center gap-2 rounded-xl border p-3 " +
              (form.role === r
                ? "border-blue-500 bg-blue-50"
                : "border-slate-200")
            }
          >
            <input
              type="radio"
              name="role"
              value={r}
              checked={form.role === r}
              onChange={(e) => setForm({ ...form, role: e.target.value })}
              className="accent-blue-600"
            />
            <span className="capitalize">{r}</span>
          </label>
        ))}
      </div>

      <button
        type="submit"
        disabled={submitting}
        className="bg-blue-600 hover:bg-blue-700 disabled:opacity-60 shadow-sm mt-2 px-4 py-2.5 rounded-xl focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-blue-300 w-full text-white transition"
      >
        {submitting ? "Creating account…" : "Sign Up"}
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
