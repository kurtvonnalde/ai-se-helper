import { type FormEvent, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { buildArtifactsPath, buildInterviewPath } from "../app/routePaths";
import { createProject, getProjects } from "../api/projectsApi";
import PageHero from "../components/PageHero";
import Panel from "../components/Panel";
import type { ProjectResponse } from "../types/project";

const ROWS_PER_PAGE = 10;

export default function ProjectsPage() {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<ProjectResponse[]>([]);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);

  const loadProjects = async () => {
    const data = await getProjects();
    setProjects(data);
  };

  useEffect(() => {
    let isActive = true;

    const loadInitialProjects = async () => {
      const data = await getProjects();

      if (isActive) {
        setProjects(data);
      }
    };

    void loadInitialProjects();

    return () => {
      isActive = false;
    };
  }, []);

  useEffect(() => {
    const totalPages = Math.max(1, Math.ceil(projects.length / ROWS_PER_PAGE));

    if (currentPage > totalPages) {
      setCurrentPage(totalPages);
    }
  }, [currentPage, projects.length]);

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

      navigate(buildInterviewPath(created.id));
    } finally {
      setLoading(false);
    }
  };

  const totalPages = Math.max(1, Math.ceil(projects.length / ROWS_PER_PAGE));
  const startIndex = (currentPage - 1) * ROWS_PER_PAGE;
  const pagedProjects = projects.slice(startIndex, startIndex + ROWS_PER_PAGE);

  return (
    <main className="page page-projects">
      <PageHero
        eyebrow="AI Project Studio"
        title="Workspace"
        description="Open an interview, review generated artifacts, or start a new project from the button above."
      />

      

      <Panel className="projects-panel reveal-up delay-2">
        <div className="panel-head projects-panel-head">
          <div>
            <h2>Projects</h2>
          </div>
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
        </div>

        {projects.length === 0 ? (
          <p className="muted">No projects yet. Create one to start the interview flow.</p>
        ) : (
          <div className="projects-table-area">
            <div className="project-table-header" aria-hidden="true">
              <span>Project</span>
              <span>Description</span>
              <span>Actions</span>
            </div>

            <ul className="project-list project-list-large project-list-table">
            {pagedProjects.map((project) => (
              <li key={project.id} className="project-item project-item-large project-item-table">
                <div className="project-cell project-title-cell">
                  <h3>{project.title}</h3>
                </div>
                <div className="project-cell project-description-cell">
                  <p>{project.description || "No description provided."}</p>
                </div>
                <div className="project-actions project-cell project-actions-cell">
                  <button
                    className="btn btn-secondary"
                    onClick={() => navigate(buildInterviewPath(project.id))}
                  >
                    Open Interview
                  </button>
                  <button
                    className="btn btn-ghost"
                    onClick={() => navigate(buildArtifactsPath(project.id))}
                  >
                    View Artifacts
                  </button>
                </div>
              </li>
            ))}
            </ul>

            <div className="pagination-bar" aria-label="Project table pagination">
              <div className="pagination-group">
                <span className="pagination-label">Rows per page:</span>
                <span className="pagination-fixed-value">{ROWS_PER_PAGE}</span>
              </div>

              <div className="pagination-group">
                <button
                  type="button"
                  className="pagination-btn"
                  onClick={() => setCurrentPage((value) => Math.max(1, value - 1))}
                  disabled={currentPage === 1}
                  aria-label="Previous page"
                >
                  &lsaquo;
                </button>
                <span className="pagination-indicator">{currentPage} / {totalPages}</span>
                <button
                  type="button"
                  className="pagination-btn"
                  onClick={() => setCurrentPage((value) => Math.min(totalPages, value + 1))}
                  disabled={currentPage === totalPages}
                  aria-label="Next page"
                >
                  &rsaquo;
                </button>
              </div>
            </div>
          </div>
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