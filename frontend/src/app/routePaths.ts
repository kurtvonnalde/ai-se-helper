export const routePaths = {
  projects: "/projects",
  interview: "/projects/:projectId/interview",
  artifacts: "/projects/:projectId/artifacts",
} as const;

export const buildInterviewPath = (projectId: string) =>
  `/projects/${projectId}/interview`;

export const buildArtifactsPath = (projectId: string) =>
  `/projects/${projectId}/artifacts`;