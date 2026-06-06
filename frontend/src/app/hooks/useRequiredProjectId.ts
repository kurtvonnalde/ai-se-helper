import { useParams } from "react-router-dom";

export function useRequiredProjectId() {
  const { projectId } = useParams<{ projectId: string }>();

  if (!projectId) {
    throw new Error("Missing projectId route parameter.");
  }

  return projectId;
}