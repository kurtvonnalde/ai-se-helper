export interface ProjectResponse {
  id: string;
  title: string;
  description: string;
  createdAtUtc: string;
}

export interface CreateProjectRequest {
  title: string;
  description: string;
}