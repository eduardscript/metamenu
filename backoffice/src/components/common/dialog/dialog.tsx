"use client";

import { useEffect, useRef } from "react";
import { useFormState } from "react-dom";
import FormSubmitButton from "./form-submit-button";
import dialogButtons from "./styles";

function useFormDialog(formAction: any) {
  const dialogRef = useRef<HTMLDialogElement>(null!);
  const formRef = useRef<HTMLFormElement>(null!);

  const [state, action] = useFormState(formAction, {
    errors: {},
    success: false,
  });

  useEffect(() => {
    if (state.success) {
      dialogRef.current.close();
      formRef.current.reset();
    }
  }, [state]);

  return { dialogRef, formRef, state, action };
}

export default function Dialog({
  formTitle,
  formAction,
  inputForm: InputFormComponent,
  buttonType,
  model,
}: {
  formTitle: string;
  formAction: any;
  inputForm: React.FC<{ state: any; model?: any }>;
  buttonType: "edit" | "create" | "delete";
  model?: any;
}) {
  const { dialogRef, formRef, state, action } = useFormDialog(formAction);

  return (
    <>
      <button
        className={`btn ${dialogButtons[buttonType].colors}`}
        onClick={() => dialogRef.current.showModal()}
      >
        {dialogButtons[buttonType].text}
      </button>
      <dialog id="create_tenant_modal" className="modal" ref={dialogRef}>
        <div className="modal-box">
          <h3 className="font-bold text-lg">{formTitle}</h3>
          <form action={action} ref={formRef}>
            <InputFormComponent state={state} model={model} />
            <div className="modal-action">
              <FormSubmitButton buttonType={buttonType} />
            </div>
          </form>
        </div>
      </dialog>
    </>
  );
}
