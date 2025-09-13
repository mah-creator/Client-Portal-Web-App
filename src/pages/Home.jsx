import About from "../features/home/About";
import FeaturedIn from "../features/home/FeaturedIn";
import Features from "../features/home/Features";
import Footer from "../features/home/Footer";
import Hero from "../features/home/Hero";
import Nav from "../features/home/Nav";

export default function Home() {
  return (
    <div className="bg-gradient-to-b from-sky-100 via-sky-50 to-sky-100 min-h-screen text-slate-800">
      <Nav />
      <Hero />
      <FeaturedIn />
      <About />
      <Features />
      <Footer />
    </div>
  );
}
