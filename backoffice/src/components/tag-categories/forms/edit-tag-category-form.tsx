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
      {state.errors?._server && (
        <span className="text-red-500 block">{state.errors._server}</span>
      )}
      <input type="hidden" name="tenantCode" value={model.tenantCode} />
      <input type="hidden" name="oldTagCategoryCode" value={model.code} />
      {state.errors?._server && (
        <span className="text-red-500 block">{state.errors._server}</span>
      )}
      <p className="py-4">
        <input
          name="newTagCategoryCode"
          className="input input-bordered"
          placeholder="tag category name"
          defaultValue={model.code}
          ref={inputRef}
        />
      </p>
      {state.errors?.name && (
        <span className="text-red-500 block">{state.errors.name}</span>
      )}
    </>
  );
}
