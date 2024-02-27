'use client'

import deleteTenant from '@/actions/tenants/delete-tenant'
import { useRef } from 'react'
import { useFormState } from 'react-dom'

export default function DeleteTenantDialog({
  title,
  children,
}: {
  children: React.ReactNode
  title: string
}) {
  const modalRef = useRef<HTMLDialogElement>(null)
  const [formState, formAction] = useFormState(deleteTenant, {})

  return (
    <>
      <button
        className="btn btn-sm bg-error text-white text-sm"
        onClick={() => modalRef.current?.showModal()}
      >
        Delete
      </button>
      <dialog ref={modalRef} className="modal">
        <div className="modal-box">
          <h3 className="font-bold text-lg">{title}</h3>
          <p className="py-4">This action can't be undone.</p>
          {formState.errors?._server && (
            <div className="alert alert-error">
              {formState.errors._server.join(', ')}
            </div>
          )}
          {formState.errors?.name && (
            <div className="alert alert-error">
              {formState.errors.name.join(', ')}
            </div>
          )}
          <form action={formAction} className="modal-backdrop">
            {children}
            <div className="flex justify-end gap-2">
              <button
                type="button"
                className="btn"
                onClick={() => modalRef.current?.close()}
              >
                close
              </button>
              <button className="btn btn-error">delete</button>
            </div>
          </form>
        </div>
      </dialog>
    </>
  )
}
