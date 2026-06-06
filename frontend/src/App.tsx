import { useState } from "react";
import axios from "axios";

function App() {
  const [message, setMessage] = useState("");
  const [chatLog, setChatLog] = useState<{ from: string; text: string }[]>([]);

  const sendMessage = async () => {
    if (!message.trim()) return;

    const newLog = [...chatLog, { from: "user", text: message }];
    setChatLog(newLog);
    setMessage("");

    try {
      const res = await axios.post("/api/chat/message", {
        message,
      });
      setChatLog([...newLog, { from: "bot", text: res.data.reply }]);
    } catch (err) {
      setChatLog([...newLog, { from: "bot", text: "Error talking to server." }]);
    }
  };

  return (
    <div style={{ padding: 20, fontFamily: "sans-serif" }}>
      <h1>AI Planning Copilot</h1>
      <div
        style={{
          border: "1px solid #ccc",
          padding: 10,
          minHeight: 300,
          marginBottom: 10,
        }}
      >
        {chatLog.map((c, i) => (
          <div key={i}>
            <strong>{c.from}:</strong> {c.text}
          </div>
        ))}
      </div>
      <input
        value={message}
        onChange={(e) => setMessage(e.target.value)}
        placeholder="Type your message..."
        style={{ width: "70%", padding: 8 }}
      />
      <button onClick={sendMessage} style={{ padding: 8, marginLeft: 8 }}>
        Send
      </button>
    </div>
  );
}

export default App;