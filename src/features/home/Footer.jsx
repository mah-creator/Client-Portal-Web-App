function Footer() {
  return (
    <footer className="bg-white/70 border-slate-200 border-t">
      <div className="flex sm:flex-row flex-col justify-between items-center gap-4 mx-auto px-6 py-6 max-w-6xl">
        <p className="text-slate-500 text-sm">
          © {new Date().getFullYear()} Client Portal
        </p>
        <div className="flex items-center gap-4 text-slate-600 text-sm">
          <a href="#about" className="hover:text-slate-900">
            About
          </a>
          <a href="#features" className="hover:text-slate-900">
            Features
          </a>
          <a href="/login" className="hover:text-slate-900">
            Log in
          </a>
        </div>
      </div>
    </footer>
  );
}

export default Footer;
