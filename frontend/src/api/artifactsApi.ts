import { api } from "./axios";
import type { GenerateArtifactsResponse } from "../types/artifact";

export const getArtifacts = async (
  projectId: string
): Promise<GenerateArtifactsResponse> => {
  const { data } = await api.get<GenerateArtifactsResponse>(
    `/projects/${projectId}/artifacts`
  );
  return data;
};

export const generateArtifacts = async (
  projectId: string
): Promise<GenerateArtifactsResponse> => {
  const { data } = await api.post<GenerateArtifactsResponse>(
    `/projects/${projectId}/artifacts/generate`
  );
  return data;
};