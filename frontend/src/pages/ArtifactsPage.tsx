import { useEffect, useState } from "react";
import ReactMarkdown from "react-markdown";
import { useNavigate } from "react-router-dom";
import { useRequiredProjectId } from "../app/hooks/useRequiredProjectId";
import { buildInterviewPath, routePaths } from "../app/routePaths";
import { generateArtifacts, getArtifacts } from "../api/artifactsApi";
import { getProjectById } from "../api/projectsApi";
import PageHero from "../components/PageHero";
import Panel from "../components/Panel";
import type { ArtifactItemResponse } from "../types/artifact";

export default function ArtifactsPage() {
  const navigate = useNavigate();
  const projectId = useRequiredProjectId();
  const [projectTitle, setProjectTitle] = useState("Project");
  const [artifacts, setArtifacts] = useState<ArtifactItemResponse[]>([]);
  const [activeArtifactType, setActiveArtifactType] = useState("");
  const [loading, setLoading] = useState(true);
  const [generating, setGenerating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let isActive = true;

    const loadArtifacts = async () => {
      try {
        setLoading(true);
        setError(null);

        const [project, artifactResponse] = await Promise.all([
          getProjectById(projectId),
          getArtifacts(projectId),
        ]);

        if (!isActive) {
          return;
        }

        setProjectTitle(project.title);
        setArtifacts(artifactResponse.artifacts);
        setActiveArtifactType((current) =>
          current && artifactResponse.artifacts.some((artifact) => artifact.artifactType === current)
            ? current
            : (artifactResponse.artifacts[0]?.artifactType ?? "")
        );
      } catch {
        if (isActive) {
          setError("Unable to load artifacts for this project.");
        }
      } finally {
        if (isActive) {
          setLoading(false);
        }
      }
    };

    void loadArtifacts();

    return () => {
      isActive = false;
    };
  }, [projectId]);

  const handleGenerate = async () => {
    try {
      setGenerating(true);
      setError(null);
      const result = await generateArtifacts(projectId);
      setArtifacts(result.artifacts);
      setActiveArtifactType((current) =>
        current && result.artifacts.some((artifact) => artifact.artifactType === current)
          ? current
          : (result.artifacts[0]?.artifactType ?? "")
      );
    } catch {
      setError("Unable to generate artifacts right now.");
    } finally {
      setGenerating(false);
    }
  };

  const activeArtifact = artifacts.find(
    (artifact) => artifact.artifactType === activeArtifactType
  ) ?? artifacts[0];

  return (
    <main className="page page-artifacts">
      <div className="top-actions reveal-up">
        <div className="action-bar">
          <button onClick={() => navigate(routePaths.projects)} className="btn btn-ghost">
            Back to Projects
          </button>
          <button onClick={() => navigate(buildInterviewPath(projectId))} className="btn btn-secondary">
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
          <div className="artifacts-tabs-layout">
            <div className="artifacts-tabs" role="tablist" aria-label="Artifact tabs">
              {artifacts.map((artifact) => {
                const isActive = artifact.artifactType === activeArtifact?.artifactType;

                return (
                  <button
                    key={artifact.artifactType}
                    type="button"
                    role="tab"
                    className={`artifact-tab ${isActive ? "artifact-tab-active" : ""}`}
                    aria-selected={isActive}
                    onClick={() => setActiveArtifactType(artifact.artifactType)}
                  >
                    {artifact.artifactType}
                  </button>
                );
              })}
            </div>

            {activeArtifact ? (
              <article className="artifact-card artifact-card-active" role="tabpanel">
                <h3>{activeArtifact.artifactType}</h3>
                <ReactMarkdown>{activeArtifact.contentMarkdown}</ReactMarkdown>
              </article>
            ) : null}
          </div>
        )}
      </Panel>
    </main>
  );
}
