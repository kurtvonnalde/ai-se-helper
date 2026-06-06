export interface ArtifactItemResponse {
  artifactType: string;
  contentMarkdown: string;
}

export interface GenerateArtifactsResponse {
  projectId: string;
  artifacts: ArtifactItemResponse[];
}

