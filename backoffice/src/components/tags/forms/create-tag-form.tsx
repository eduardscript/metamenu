"use client";

export default function CreateTagForm({
  state,
  model,
}: {
  state: {
    errors?: {
      code?: string[];
      _server?: string[];
    };
  };
  model?: any;
}) {
  return (
    <>
      <input type="hidden" name="tenantCode" value={model.tenantCode} />
      {state.errors?._server && (
        <span className="text-red-500 block">{state.errors._server}</span>
      )}
      <input
        type="hidden"
        name="tagCategoryCode"
        value={model.tagCategoryCode}
      />
      <input
        name="code"
        className="input input-bordered"
        placeholder="tag code"
      />
      {state.errors?.code && (
        <span className="text-red-500 block">{state.errors.code}</span>
      )}
    </>
  );
}
