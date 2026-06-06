import { type FormEvent, useEffect, useRef, useState } from "react";
import {
  getAnswers,
  getCurrentQuestion,
  startInterview,
  submitAnswer,
} from "../api/interviewApi";
import { generateArtifacts } from "../api/artifactsApi";
import CountPill from "../components/CountPill";
import PageHero from "../components/PageHero";
import Panel from "../components/Panel";
import type {
  InterviewAnswerResponse,
  SubmitAnswerResponse,
} from "../types/interview";

interface InterviewPageProps {
  projectId: string;
  onBack: () => void;
  onOpenArtifacts: () => void;
}

export default function InterviewPage({
  projectId,
  onBack,
  onOpenArtifacts,
}: InterviewPageProps) {
  const [sessionId, setSessionId] = useState("");
  const [questionKey, setQuestionKey] = useState("");
  const [questionText, setQuestionText] = useState("");
  const [answerText, setAnswerText] = useState("");
  const [answers, setAnswers] = useState<InterviewAnswerResponse[]>([]);
  const [sessionCompleted, setSessionCompleted] = useState(false);
  const [loading, setLoading] = useState(false);
  const [generating, setGenerating] = useState(false);
  const chatThreadRef = useRef<HTMLDivElement | null>(null);

  const loadAnswers = async () => {
    const data = await getAnswers(projectId);
    setAnswers(data);
  };

  const initializeInterview = async () => {
    try {
      setLoading(true);

      const start = await startInterview(projectId);
      setSessionId(start.sessionId);
      setQuestionKey(start.questionKey);
      setQuestionText(start.questionText);
      setSessionCompleted(false);

      await loadAnswers();
    } catch {
      const current = await getCurrentQuestion(projectId);
      setSessionId(current.sessionId);
      setQuestionKey(current.questionKey);
      setQuestionText(current.questionText);
      setSessionCompleted(current.sessionCompleted);

      await loadAnswers();
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    initializeInterview();
  }, [projectId]);

  useEffect(() => {
    chatThreadRef.current?.scrollTo({
      top: chatThreadRef.current.scrollHeight,
      behavior: "smooth",
    });
  }, [answers, questionText, sessionCompleted]);

  const handleSubmitAnswer = async (e: FormEvent) => {
    e.preventDefault();

    if (!answerText.trim()) return;

    try {
      setLoading(true);

      const result: SubmitAnswerResponse = await submitAnswer(projectId, {
        sessionId,
        questionKey,
        answerText,
      });

      setAnswerText("");
      await loadAnswers();

      if (result.sessionCompleted) {
        setSessionCompleted(true);
        setQuestionKey("");
        setQuestionText("");
      } else {
        setQuestionKey(result.nextQuestionKey ?? "");
        setQuestionText(result.nextQuestionText ?? "");
      }
    } finally {
      setLoading(false);
    }
  };

  const handleGenerateArtifacts = async () => {
    try {
      setGenerating(true);
      await generateArtifacts(projectId);
      onOpenArtifacts();
    } finally {
      setGenerating(false);
    }
  };

  const chatMessages = [
    {
      id: "intro",
      role: "assistant",
      title: "Planning Assistant",
      text: "I will guide you through the planning interview one message at a time. Reply in the chat composer below and I will move to the next prompt.",
    },
    ...answers.flatMap((item, index) => [
      {
        id: `question-${item.questionKey}-${index}`,
        role: "assistant",
        title: "Assistant",
        text: item.questionText,
      },
      {
        id: `answer-${item.questionKey}-${item.createdAtUtc}`,
        role: "user",
        title: "You",
        text: item.answerText,
      },
    ]),
  ];

  if (sessionCompleted) {
    chatMessages.push({
      id: "completed",
      role: "system",
      title: "Interview Complete",
      text: "All interview prompts are complete. You can generate planning artifacts when you are ready.",
    });
  } else if (questionText) {
    chatMessages.push({
      id: `current-${questionKey}`,
      role: "assistant",
      title: "Assistant",
      text: questionText,
    });
  } else if (loading) {
    chatMessages.push({
      id: "loading",
      role: "system",
      title: "Loading",
      text: "Preparing the next prompt...",
    });
  }

  return (
    <main className="page page-interview">
      <div className="top-actions reveal-up">
        <div className="action-bar">
          <button onClick={onBack} className="btn btn-ghost">
            Back to Projects
          </button>
          <button onClick={onOpenArtifacts} className="btn btn-secondary">
            Go to Artifacts
          </button>
        </div>
      </div>

      <PageHero eyebrow="Guided Session" title="Planning Interview" className="delay-1" />

      <section className="grid-two interview-grid">
        <Panel className="interview-chat-panel reveal-up delay-1">
          <div className="panel-head chat-panel-head">
            <div>
              <h2>Interview Chat</h2>
              <p className="muted chat-panel-copy">
                Answer naturally. Each reply advances the planning conversation.
              </p>
            </div>
          </div>

          <div ref={chatThreadRef} className="chat-thread" aria-live="polite">
            {chatMessages.map((message) => (
              <article
                key={message.id}
                className={`chat-message chat-message-${message.role}`}
              >
                <p className="chat-message-title">{message.title}</p>
                <p>{message.text}</p>
              </article>
            ))}
          </div>

          {!sessionCompleted ? (
            <form onSubmit={handleSubmitAnswer} className="chat-composer">
              <textarea
                value={answerText}
                onChange={(e) => setAnswerText(e.target.value)}
                rows={4}
                className="field-input field-textarea chat-input"
                placeholder="Type your reply here..."
              />

              <div className="chat-composer-actions">
                <p className="muted helper-text chat-helper-text">
                  Artifacts stay in a separate page after the interview is complete.
                </p>
                <button type="submit" className="btn btn-primary" disabled={loading}>
                  {loading ? "Sending..." : "Send Reply"}
                </button>
              </div>
            </form>
          ) : (
            <div className="complete-card chat-complete-card">
              <h2>Ready to Generate</h2>
              <p>The conversation is complete. Generate the planning artifacts when you are ready.</p>

              <button onClick={handleGenerateArtifacts} className="btn btn-primary" disabled={generating}>
                {generating ? "Generating..." : "Generate Plan and Open"}
              </button>
            </div>
          )}
        </Panel>

        <Panel className="reveal-up delay-2" as="aside">
          <div className="panel-head">
            <h2>Session Status</h2>
            <CountPill count={answers.length} />
          </div>

          <div className="answers-list interview-status-list">
            <div className="answer-item interview-status-item">
              <strong>Responses Saved</strong>
              <p>{answers.length} message{answers.length === 1 ? "" : "s"} captured in this interview.</p>
            </div>

            <div className="answer-item interview-status-item">
              <strong>Current State</strong>
              <p>{sessionCompleted ? "Interview complete." : "Waiting for your next chat reply."}</p>
            </div>

            <div className="answer-item interview-status-item">
              <strong>Next Step</strong>
              <p>
                {sessionCompleted
                  ? "Generate artifacts or review the completed interview transcript."
                  : "Reply to the latest assistant message to continue the planning flow."}
              </p>
            </div>
          </div>
        </Panel>
      </section>
    </main>
  );
}