import { useState } from "react";
import { useNavigate } from "react-router";
import { useAuth } from "../../contexts/AuthContext";

const roles = [
  {
    id: "freelancer",
    title: "Freelancer",
    subtitle: "Manage projects and tasks",
    icon: (
      <svg
        viewBox="0 0 24 24"
        className="w-5 h-5"
        fill="none"
        stroke="currentColor"
        strokeWidth="1.8"
      >
        <path d="M7 21v-2a4 4 0 0 1 4-4h2a4 4 0 0 1 4 4v2" />
        <circle cx="11" cy="7" r="4" />
      </svg>
    ),
  },
  {
    id: "customer",
    title: "Customer",
    subtitle: "View project progress",
    icon: (
      <svg
        viewBox="0 0 24 24"
        className="w-5 h-5"
        fill="none"
        stroke="currentColor"
        strokeWidth="1.8"
      >
        <path d="M3 3h18v4H3z" />
        <path d="M5 7v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V7" />
        <path d="M8 11h8M8 15h5" />
      </svg>
    ),
  },
];

function LoginForm() {
  const [role, setRole] = useState("freelancer");
  const [form, setForm] = useState({ email: "", password: "" });
  const [error, setError] = useState("");
  const [submitting, setSubmitting] = useState(false);

  const { login } = useAuth();

  const navigate = useNavigate();

  // LoginForm.jsx (essentials)
  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSubmitting(true);
    try {
      const user = await login(form.email.trim(), form.password); // 👈 no role
      if (user.role === "freelancer")
        navigate("/freelancer", { replace: true });
      else if (user.role === "customer")
        navigate("/customer", { replace: true });
      else {
        navigate("/admin", { replace: true });
      }
    } catch (err) {
      setError(err.message || "Login failed");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form className="mt-8" onSubmit={handleSubmit}>
      {error && (
        <div className="bg-rose-50 mb-3 px-3 py-2 border border-rose-200 rounded-lg text-rose-700 text-sm">
          {error}
        </div>
      )}

      <label className="block font-medium text-slate-700 text-sm">Email</label>
      <input
        type="email"
        placeholder="Enter your email"
        className="bg-white shadow-sm mt-1 px-4 py-2.5 border border-slate-200 focus:border-blue-500 rounded-xl outline-none focus:ring-4 focus:ring-blue-200 w-full text-slate-900 placeholder-slate-400"
        value={form.email}
        onChange={(e) => setForm({ ...form, email: e.target.value })}
        required
      />

      <label className="block mt-4 font-medium text-slate-700 text-sm">
        Password
      </label>
      <input
        type="password"
        placeholder="Enter your password"
        className="bg-white shadow-sm mt-1 px-4 py-2.5 border border-slate-200 focus:border-blue-500 rounded-xl outline-none focus:ring-4 focus:ring-blue-200 w-full text-slate-900 placeholder-slate-400"
        value={form.password}
        onChange={(e) => setForm({ ...form, password: e.target.value })}
        required
      />

      {/* <p className="mt-5 font-semibold text-slate-700 text-sm">Login as:</p>
      <div
        role="radiogroup"
        aria-label="Login role"
        className="gap-3 grid mt-2"
      >
        {roles.map((r) => {
          const selected = role === r.id;
          return (
            <button
              key={r.id}
              type="button"
              role="radio"
              aria-checked={selected}
              onClick={() => setRole(r.id)}
              className={
                "group flex w-full items-start gap-3 rounded-2xl border p-4 text-left transition focus:outline-none " +
                (selected
                  ? "border-blue-500 bg-blue-50 ring-4 ring-blue-200"
                  : "border-slate-200 hover:border-slate-300")
              }
            >
              <span
                className={
                  "mt-0.5 inline-flex h-9 w-9 items-center justify-center rounded-xl " +
                  (selected
                    ? "bg-blue-500 text-white"
                    : "bg-slate-100 text-slate-600")
                }
              >
                {r.icon}
              </span>
              <span>
                <span className="block font-medium text-slate-800">
                  {r.title}
                </span>
                <span className="text-slate-500 text-sm">{r.subtitle}</span>
              </span>
            </button>
          );
        })}
      </div> */}

      <button
        type="submit"
        disabled={submitting}
        className="bg-blue-600 hover:bg-blue-700 disabled:opacity-60 shadow-sm mt-6 px-4 py-2.5 rounded-xl w-full text-white transition"
      >
        {submitting ? "Signing in…" : "Sign In"}
      </button>

      <p className="mt-4 text-slate-600 text-sm text-center">
        Don’t have an account?{" "}
        <a
          onClick={() => navigate("/signup")}
          className="font-medium text-blue-600 hover:text-blue-700 cursor-pointer"
        >
          Sign up
        </a>
      </p>
    </form>
  );
}

export default LoginForm;
