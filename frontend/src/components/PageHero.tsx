interface PageHeroProps {
  eyebrow: string;
  title: string;
  description?: string;
  className?: string;
}

export default function PageHero({
  eyebrow,
  title,
  description,
  className = "",
}: PageHeroProps) {
  const classes = className.trim()
    ? `hero-card reveal-up ${className}`
    : "hero-card reveal-up";

  return (
    <section className={classes}>
      <p className="eyebrow">{eyebrow}</p>
      <h1>{title}</h1>
      {description ? <p className="hero-copy">{description}</p> : null}
    </section>
  );
}
