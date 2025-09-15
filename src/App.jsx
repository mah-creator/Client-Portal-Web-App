import { BrowserRouter, Navigate, Route, Routes } from "react-router";
import Login from "./pages/login";
import AppLayout from "./pages/AppLayout";
import Home from "./pages/Home";
import Freelancer from "./pages/Freelancer";
import Customer from "./pages/Customer";
import Admin from "./pages/Admin";
import Signup from "./pages/Signup";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<AppLayout />}>
          <Route index element={<Navigate replace to="home" />} />
          <Route path="home" element={<Home />} />
          <Route path="login" element={<Login />} />
          <Route path="signup" element={<Signup />} />
          <Route path="freelancer" element={<Freelancer />} />
          <Route path="customer" element={<Customer />} />
          <Route path="admin" element={<Admin />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
