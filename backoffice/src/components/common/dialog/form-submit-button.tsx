"use client";

import { useFormStatus } from "react-dom";
import dialogButtons from "./styles";

export default function FormSubmitButton({
  onClick,
  buttonType,
}: {
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
  buttonType: "edit" | "create" | "delete";
}) {
  const status = useFormStatus();

  const button = dialogButtons[buttonType];

  return (
    <button
      className={
        status.pending
          ? "text-white loading loading-dots loading-lg "
          : "btn " + button.colors
      }
      type="submit"
      disabled={status.pending}
      onClick={onClick}
    >
      {button.text}
    </button>
  );
}
