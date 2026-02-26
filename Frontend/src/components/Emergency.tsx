import { useState } from "react";
import "../style/emergency.css";
type EmergencyProps = {
  open: boolean;
  onClose: () => void;
};

export default function Emergency({ open, onClose }: EmergencyProps) {
  const [description, setDescription] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async () => {
    if (!description.trim()) return;

    try {
      setLoading(true);

      const response = await fetch(
        "https://localhost:7001/api/ai/appointments/schedule",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ text: description }),
        }
      );

      if (!response.ok) {
        throw new Error("API request failed");
      }

      const data = await response.json();
      console.log("API response:", data);

      setDescription("");
      onClose();
    } catch (error) {
      console.error("Emergency error:", error);
    } finally {
      setLoading(false);
    }
  };

  if (!open) return null;

  return (
    <div className="overlay">
      <div className="modal">
        <h3>🚑 Emergency Request</h3>

        <textarea
          placeholder="Enter emergency description..."
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />

        <div className="buttons">
          <button onClick={onClose}>Cancel</button>

          <button onClick={handleSubmit} disabled={loading}>
            {loading ? "Sending..." : "Submit"}
          </button>
        </div>
      </div>
    </div>
  );
}