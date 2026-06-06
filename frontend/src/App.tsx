import { useState } from "react";
import "./App.css";
import ProjectsPage from "./pages/ProjectsPage";
import InterviewPage from "./pages/InterviewPage";
import ArtifactsPage from "./pages/ArtifactsPage";

type AppView = "projects" | "interview" | "artifacts";

export default function App() {
  const [selectedProjectId, setSelectedProjectId] = useState<string | null>(null);
  const [activeView, setActiveView] = useState<AppView>("projects");

  const handleOpenInterview = (projectId: string) => {
    setSelectedProjectId(projectId);
    setActiveView("interview");
  };

  const handleOpenArtifacts = (projectId: string) => {
    setSelectedProjectId(projectId);
    setActiveView("artifacts");
  };

  const handleBackToProjects = () => {
    setActiveView("projects");
  };

  const content = activeView === "projects" || !selectedProjectId ? (
    <ProjectsPage
      onSelectProject={handleOpenInterview}
      onOpenArtifacts={handleOpenArtifacts}
    />
  ) : activeView === "interview" ? (
    <InterviewPage
      projectId={selectedProjectId}
      onBack={handleBackToProjects}
      onOpenArtifacts={() => setActiveView("artifacts")}
    />
  ) : (
    <ArtifactsPage
      projectId={selectedProjectId}
      onBack={handleBackToProjects}
      onOpenInterview={() => setActiveView("interview")}
    />
  );

  return (
    <div className="app-shell">
      <div className="ambient ambient-one" aria-hidden="true" />
      <div className="ambient ambient-two" aria-hidden="true" />
      {content}
    </div>
  );
}