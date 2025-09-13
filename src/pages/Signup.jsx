import { useState } from "react";
import SuccessSignUp from "../features/signup/SuccessSignUp";
import SignupHeader from "../features/signup/SignupHeader";
import SignupForm from "../features/signup/SignupForm";

export default function Signup() {
  const [success, setSuccess] = useState(false);
  const [form, setForm] = useState({
    name: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  if (success) {
    return <SuccessSignUp email={form.email} />;
  }

  return (
    <div className="flex justify-center items-center bg-gradient-to-b from-sky-100 via-sky-50 to-sky-100 p-4 w-full min-h-screen">
      <div className="w-full max-w-md">
        <div className="bg-white/90 shadow-xl backdrop-blur-sm mx-auto p-8 rounded-3xl ring-1 ring-black/5">
          <SignupHeader />
          <SignupForm form={form} setForm={setForm} setSuccess={setSuccess} />
        </div>
      </div>
    </div>
  );
}
