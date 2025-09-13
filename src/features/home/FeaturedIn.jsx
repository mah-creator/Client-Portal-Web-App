function FeaturedIn() {
  return (
    <section aria-label="Featured In" className="bg-white/80">
      <div className="mx-auto px-6 py-8 max-w-6xl">
        <p className="mb-4 text-slate-500 text-xs text-center uppercase tracking-widest">
          Featured in
        </p>
        <div className="items-center gap-6 grid grid-cols-2 sm:grid-cols-3 md:grid-cols-6 opacity-70">
          {[
            "DevWeek",
            "ProductHunt",
            "HashNode",
            "Smashing",
            "IndieHackers",
            "FreeCodeCamp",
          ].map((brand) => (
            <div
              key={brand}
              className="flex justify-center items-center bg-white px-3 py-2 border border-slate-100 rounded-lg text-sm"
            >
              {brand}
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

export default FeaturedIn;
