"use client";

import { FC, useEffect, useRef, useState } from "react";
import SelectTagCategory from "../select-tag-category";

const EditTagForm: FC<{ state: any; model?: any }> = ({ state, model }) => {
  const inputRef = useRef<HTMLInputElement>(null);

  const { tag, tagCategories } = model;

  const [tagCategoryCode, setTagCategoryCode] = useState<string>(
    model.tagCategoryCode
  );
  const [tagCode, setTagCode] = useState<string>(tag.code);

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
      {JSON.stringify(model)}
      <input name="tenantCode" type="hidden" value={model.tenantCode} />
      <input name="code" type="hidden" value={tag.code} />
      <input name="newTagCategoryCode" type="hidden" value={tagCategoryCode} />
      {state.errors?._server && (
        <span className="text-red-500 block">{state.errors._server}</span>
      )}
      <p className="py-4">
        <input
          name="newTagCode"
          className="input input-bordered"
          defaultValue={tag.code}
          ref={inputRef}
          onChange={(e) => setTagCode(e.target.value)}
        />
      </p>
      {state.errors?.name && (
        <span className="text-red-500 block">{state.errors.name}</span>
      )}
      <p>
        <label className="label">Tag Category</label>
        <SelectTagCategory
          isQuery={false}
          tagCategories={tagCategories}
          onChange={(value) => {
            setTagCategoryCode(value);
          }}
        />
      </p>
    </>
  );
};

export default EditTagForm;
