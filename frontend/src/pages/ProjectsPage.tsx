import { type FormEvent, useEffect, useState } from "react";
import { createProject, getProjects } from "../api/projectsApi";
import CountPill from "../components/CountPill";
import PageHero from "../components/PageHero";
import Panel from "../components/Panel";
import type { ProjectResponse } from "../types/project";

interface ProjectsPageProps {
  onSelectProject: (projectId: string) => void;
  onOpenArtifacts: (projectId: string) => void;
}

export default function ProjectsPage({
  onSelectProject,
  onOpenArtifacts,
}: ProjectsPageProps) {
  const [projects, setProjects] = useState<ProjectResponse[]>([]);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [loading, setLoading] = useState(false);

  const loadProjects = async () => {
    const data = await getProjects();
    setProjects(data);
  };

  useEffect(() => {
    loadProjects();
  }, []);

  const handleCreateProject = async (e: FormEvent) => {
    e.preventDefault();

    if (!title.trim()) return;

    try {
      setLoading(true);
      const created = await createProject({
        title,
        description,
      });

      setTitle("");
      setDescription("");
      setIsCreateModalOpen(false);
      await loadProjects();

      onSelectProject(created.id);
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="page page-projects">
      <PageHero
        eyebrow="AI Planning Studio"
        title="Project Workspace"
        description="Capture project context, run guided interviews, and generate planning artifacts in one flow."
      />

      <div className="top-actions reveal-up delay-1">
        <div className="action-bar">
          <button
            type="button"
            className="btn btn-primary btn-wide"
            onClick={() => setIsCreateModalOpen(true)}
          >
            Create Project
          </button>
        </div>
      </div>

      <Panel className="projects-panel reveal-up delay-2">
        <div className="panel-head projects-panel-head">
          <div>
            <h2>Existing Projects</h2>
            <p className="muted projects-panel-copy">
              Open an interview, review generated artifacts, or start a new project from the button above.
            </p>
          </div>
          <CountPill count={projects.length} />
        </div>

        {projects.length === 0 ? (
          <p className="muted">No projects yet. Create one to start the interview flow.</p>
        ) : (
          <ul className="project-list project-list-large">
            {projects.map((project) => (
              <li key={project.id} className="project-item project-item-large">
                <div>
                  <h3>{project.title}</h3>
                  <p>{project.description || "No description provided."}</p>
                </div>
                <div className="project-actions">
                  <button className="btn btn-secondary" onClick={() => onSelectProject(project.id)}>
                    Open Interview
                  </button>
                  <button className="btn btn-ghost" onClick={() => onOpenArtifacts(project.id)}>
                    View Artifacts
                  </button>
                </div>
              </li>
            ))}
          </ul>
        )}
      </Panel>

      {isCreateModalOpen ? (
        <div
          className="modal-backdrop"
          role="presentation"
          onClick={() => !loading && setIsCreateModalOpen(false)}
        >
          <div
            className="modal-card reveal-up"
            role="dialog"
            aria-modal="true"
            aria-labelledby="create-project-modal-title"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="panel-head modal-head">
              <div>
                <p className="eyebrow">New Project</p>
                <h2 id="create-project-modal-title">Create Project</h2>
              </div>
              <button
                type="button"
                className="btn btn-ghost"
                onClick={() => setIsCreateModalOpen(false)}
                disabled={loading}
              >
                Close
              </button>
            </div>

            <form onSubmit={handleCreateProject} className="stack-form">
              <label className="field-label" htmlFor="project-title">Project Title</label>
              <input
                id="project-title"
                className="field-input"
                type="text"
                placeholder="Ex: POS App Checkout"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
              />

              <label className="field-label" htmlFor="project-description">Description</label>
              <textarea
                id="project-description"
                className="field-input field-textarea"
                placeholder="Key goals, constraints, and timelines"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                rows={5}
              />

              <div className="modal-actions">
                <button
                  type="button"
                  className="btn btn-ghost"
                  onClick={() => setIsCreateModalOpen(false)}
                  disabled={loading}
                >
                  Cancel
                </button>
                <button type="submit" className="btn btn-primary" disabled={loading}>
                  {loading ? "Creating..." : "Create Project"}
                </button>
              </div>
            </form>
          </div>
        </div>
      ) : null}
    </main>
  );
}