import { useState } from "react";
import SuccessSignUp from "../features/signup/SuccessSignUp";
import SignupHeader from "../features/signup/SignupHeader";
import SignupForm from "../features/signup/SignupForm";

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
    return <SuccessSignUp />;
  }

  return (
    <div className="flex justify-center items-center bg-gradient-to-b from-sky-100 via-sky-50 to-sky-100 p-4 w-full min-h-screen">
      <div className="w-full max-w-md">
        <div className="bg-white/90 shadow-xl backdrop-blur-sm mx-auto p-8 rounded-3xl ring-1 ring-black/5">
          <SignupHeader />
          <SignupForm />
        </div>
      </div>
    </div>
  );
}
