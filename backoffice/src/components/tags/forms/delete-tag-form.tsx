"use client";

import { FC, useEffect, useRef } from "react";

const DeleteTagForm: FC<{ state: any; model?: any }> = ({ state, model }) => {
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
      <input name="tenantCode" type="hidden" value={model.tenantCode} />
      <input
        name="tagCategoryCode"
        type="hidden"
        value={model.tagCategoryCode}
      />
      {state.errors?._server && (
        <span className="text-red-500 block">{state.errors._server}</span>
      )}
      <input name="code" type="hidden" value={model.code} />
      <p className="py-4">
        Are you sure you want to delete the tag {model.code}?
      </p>
      <p className="font-bold text-sm italic">(This action cannot be undone)</p>
    </>
  );
};

export default DeleteTagForm;
