import LoginForm from "../features/login/LoginForm";
import LoginHeader from "../features/login/LoginHeader";

export default function Login() {
  return (
    <div className="flex justify-center items-center bg-gradient-to-b from-sky-100 via-sky-50 to-sky-100 p-4 w-full min-h-screen">
      <div className="w-full max-w-md">
        <div className="bg-white/90 shadow-xl backdrop-blur-sm mx-auto p-8 rounded-3xl ring-1 ring-black/5">
          <LoginHeader />
          <LoginForm />
        </div>
      </div>
    </div>
  );
}
