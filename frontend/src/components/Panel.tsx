import type { ElementType, ReactNode } from "react";

interface PanelProps {
  children: ReactNode;
  className?: string;
  as?: ElementType;
}

export default function Panel({
  children,
  className = "",
  as: Component = "article",
}: PanelProps) {
  const classes = className.trim() ? `panel ${className}` : "panel";
  return <Component className={classes}>{children}</Component>;
}
