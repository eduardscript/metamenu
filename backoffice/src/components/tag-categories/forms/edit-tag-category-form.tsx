"use client";

import { useEffect, useRef } from "react";

export default function EditTagCategoryForm({
  state,
  model,
}: {
  state: {
    errors?: {
      name?: string[];
      _server?: string[];
    };
  };
  model?: any;
}) {
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
      <input type="hidden" name="tenantId" value={model.tenantId} />
      <input type="hidden" name="id" value={model.id} />
      {state.errors?._server && (
        <span className="text-red-500 block">{state.errors._server}</span>
      )}
      <p className="py-4">
        <input
          name="name"
          className="input input-bordered"
          placeholder="tag category name"
          defaultValue={model.name}
          ref={inputRef}
        />
      </p>
      {state.errors?.name && (
        <span className="text-red-500 block">{state.errors.name}</span>
      )}
    </>
  );
}
