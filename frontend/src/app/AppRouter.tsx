import {
  createBrowserRouter,
  Navigate,
  RouterProvider,
} from "react-router-dom";
import ArtifactsPage from "../pages/ArtifactsPage";
import InterviewPage from "../pages/InterviewPage";
import ProjectsPage from "../pages/ProjectsPage";
import AppLayout from "./AppLayout";
import { routePaths } from "./routePaths";

const router = createBrowserRouter([
  {
    path: "/",
    element: <AppLayout />,
    children: [
      {
        index: true,
        element: <Navigate to={routePaths.projects} replace />,
      },
      {
        path: routePaths.projects,
        element: <ProjectsPage />,
      },
      {
        path: routePaths.interview,
        element: <InterviewPage />,
      },
      {
        path: routePaths.artifacts,
        element: <ArtifactsPage />,
      },
      {
        path: "*",
        element: <Navigate to={routePaths.projects} replace />,
      },
    ],
  },
]);

export default function AppRouter() {
  return <RouterProvider router={router} />;
}