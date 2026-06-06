import {api} from "./axios";
import type { CreateProjectRequest, ProjectResponse } from "../types/project";


export const createProject = async (
  payload: CreateProjectRequest
): Promise<ProjectResponse> => {
  const { data } = await api.post<ProjectResponse>("/projects", payload);
  return data;
};


export const getProjects = async (): Promise<ProjectResponse[]> => {
  const { data } = await api.get<ProjectResponse[]>("/projects");
  return data;
};

export const getProjectById = async (projectId: string): Promise<ProjectResponse> => {
  const { data } = await api.get<ProjectResponse>(`/projects/${projectId}`);
  return data;
};
