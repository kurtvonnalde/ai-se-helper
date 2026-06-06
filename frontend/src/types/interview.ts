export interface StartInterviewResponse {
  sessionId: string;
  currentQuestionOrder: number;
  questionKey: string;
  questionText: string;
}

export interface CurrentQuestionResponse {
  sessionId: string;
  currentQuestionOrder: number;
  questionKey: string;
  questionText: string;
  sessionCompleted: boolean;
}

export interface SubmitAnswerRequest {
  sessionId: string;
  questionKey: string;
  answerText: string;
}

export interface SubmitAnswerResponse {
  sessionCompleted: boolean;
  nextQuestionOrder: number | null;
  nextQuestionKey: string | null;
  nextQuestionText: string | null;
}

export interface InterviewAnswerResponse {
  questionKey: string;
  questionText: string;
  answerText: string;
  createdAtUtc: string;
}