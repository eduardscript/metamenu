"use client";

import { FC, useEffect, useRef } from "react";

const DeleteTenantForm: FC<{ state: any; model?: any }> = ({
  state,
  model,
}) => {
  const inputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    if (inputRef.current) {
      const inputElement = inputRef.current;
      inputElement.focus();
      const valueLength = inputElement.value.length;
      inputElement.setSelectionRange(valueLength, valueLength);
    }
  }, []);

  return (
    <>
      {state.errors?.name && (
        <span className="text-red-500 block">{state.errors.name}</span>
      )}
      <input name="code" type="hidden" value={model.code} />
      <p className="py-4">
        Are you sure you want to delete tenant {model.code}?
      </p>
      <p className="font-bold text-sm italic">(This action cannot be undone)</p>
    </>
  );
};

export default DeleteTenantForm;
