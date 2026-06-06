import { useEffect, useState } from "react";
import ReactMarkdown from "react-markdown";
import { generateArtifacts, getArtifacts } from "../api/artifactsApi";
import { getProjectById } from "../api/projectsApi";
import PageHero from "../components/PageHero";
import Panel from "../components/Panel";
import type { ArtifactItemResponse } from "../types/artifact";

interface ArtifactsPageProps {
  projectId: string;
  onBack: () => void;
  onOpenInterview: () => void;
}

export default function ArtifactsPage({
  projectId,
  onBack,
  onOpenInterview,
}: ArtifactsPageProps) {
  const [projectTitle, setProjectTitle] = useState("Project");
  const [artifacts, setArtifacts] = useState<ArtifactItemResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [generating, setGenerating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadArtifacts = async () => {
    try {
      setLoading(true);
      setError(null);

      const [project, artifactResponse] = await Promise.all([
        getProjectById(projectId),
        getArtifacts(projectId),
      ]);

      setProjectTitle(project.title);
      setArtifacts(artifactResponse.artifacts);
    } catch {
      setError("Unable to load artifacts for this project.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadArtifacts();
  }, [projectId]);

  const handleGenerate = async () => {
    try {
      setGenerating(true);
      setError(null);
      const result = await generateArtifacts(projectId);
      setArtifacts(result.artifacts);
    } catch {
      setError("Unable to generate artifacts right now.");
    } finally {
      setGenerating(false);
    }
  };

  return (
    <main className="page page-artifacts">
      <div className="top-actions reveal-up">
        <div className="action-bar">
          <button onClick={onBack} className="btn btn-ghost">
            Back to Projects
          </button>
          <button onClick={onOpenInterview} className="btn btn-secondary">
            Go to Interview
          </button>
          <button onClick={handleGenerate} className="btn btn-primary" disabled={generating}>
            {generating ? "Generating..." : "Generate / Regenerate"}
          </button>
        </div>
      </div>

      <PageHero
        eyebrow="Generated Output"
        title={`${projectTitle} Artifacts`}
        description="Review the latest generated artifacts. Regenerating creates a fresh latest version per artifact type."
        className="delay-1"
      />

      <Panel className="reveal-up delay-2">
        {loading ? (
          <p className="muted">Loading artifacts...</p>
        ) : error ? (
          <p className="muted">{error}</p>
        ) : artifacts.length === 0 ? (
          <div className="empty-state">
            <h2>No generated artifacts yet</h2>
            <p className="muted">
              Generate artifacts from here or complete an interview session first.
            </p>
          </div>
        ) : (
          <div className="artifact-list">
            {artifacts.map((artifact) => (
              <article key={artifact.artifactType} className="artifact-card">
                <h3>{artifact.artifactType}</h3>
                <ReactMarkdown>{artifact.contentMarkdown}</ReactMarkdown>
              </article>
            ))}
          </div>
        )}
      </Panel>
    </main>
  );
}
