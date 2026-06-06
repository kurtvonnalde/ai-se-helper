import { api } from "./axios";
import type {
  StartInterviewResponse,
  CurrentQuestionResponse,
  SubmitAnswerRequest,
  SubmitAnswerResponse,
  InterviewAnswerResponse,
} from "../types/interview";

export const startInterview = async (
  projectId: string
): Promise<StartInterviewResponse> => {
  const { data } = await api.post<StartInterviewResponse>(
    `/projects/${projectId}/interview/start`
  );
  return data;
};

export const getCurrentQuestion = async (
  projectId: string
): Promise<CurrentQuestionResponse> => {
  const { data } = await api.get<CurrentQuestionResponse>(
    `/projects/${projectId}/interview/current`
  );
  return data;
};

export const submitAnswer = async (
  projectId: string,
  payload: SubmitAnswerRequest
): Promise<SubmitAnswerResponse> => {
  const { data } = await api.post<SubmitAnswerResponse>(
    `/projects/${projectId}/interview/answer`,
    payload
  );
  return data;
};

export const getAnswers = async (
  projectId: string
): Promise<InterviewAnswerResponse[]> => {
  const { data } = await api.get<InterviewAnswerResponse[]>(
    `/projects/${projectId}/interview/answers`
  );
  return data;
};