import React, { useMemo } from "react";
import { useDispatch } from "react-redux";
import { useInvoices } from "../hooks/useInvoices";
import useFetchInvoices from "../hooks/useFetchInvoices";

const InvoicesListComponent = () => {
    useFetchInvoices();
    const dispatch = useDispatch();
    const { invoices } = useInvoices();

    const handleSupplierClick = (supplierId) => {
        window.location.href = `/admin/supplier/update/${supplierId}`;
    };
    
    const handleInvoiceClick = (invoiceId) => {
        window.location.href = `/admin/invoice/update/${invoiceId}`;
    };
    const handleManagerClick = (managerId) => {
        window.location.href = `/admin/user/update/${managerId}`;
    };

    const renderInvoices = useMemo(() => {
        if (!invoices || invoices.length === 0) {
            return (
                <tr>
                    <td colSpan="6" className="text-center">
                        
                    </td>
                </tr>
            );
        }

        return invoices.map((invoice) => (
            <tr key={invoice.id} style={{ cursor: "pointer", verticalAlign: "middle" }}>
             
                <td
                    className="text-center"
                    onClick={() => handleInvoiceClick(invoice.id)}
                >
                    {invoice.id}
                </td>

             
                <td className="text-start">
                    <div
                        onClick={() => handleSupplierClick(invoice.supplierId)}
                        style={{ cursor: "pointer", fontWeight: "bold" }}
                    >
                        {invoice.companyName || "—"}
                    </div>
                    <div
                        onClick={() => handleManagerClick(invoice.managerId)}
                        style={{ cursor: "pointer" }}
                    >
                      {invoice.managerId}
                    </div>
                </td>

                <td
                    className="text-start"
                    onClick={() => handleInvoiceClick(invoice.id)}
                >
                    <div>Получено: {new Date(invoice.dateReceived).toLocaleDateString()}</div>
                    <div>Оплачено: {invoice.paymentDate ? new Date(invoice.paymentDate).toLocaleDateString() : "—"}</div>
                </td>

             
                <td
                    className="text-start"
                    onClick={() => handleInvoiceClick(invoice.id)}
                >
                    <div>{invoice.totalAmount} ₽</div>
                    <div>{invoice.status}</div>
                </td>

              
                <td
                    className="text-start"
                    onClick={() => handleInvoiceClick(invoice.id)}
                >
                    <div>{invoice.phone || "—"}</div>
                    <div>{invoice.email || "—"}</div>
                </td>
            </tr>
        ));
    }, [invoices]);

    return (
        <table className="table table-striped table-hover table-responsive-md table-sm">
            <thead className="thead-dark">
                <tr>
                    <th className="text-center">ID</th>
                    <th className="text-center">Компания и Менеджер</th>
                    <th className="text-center">Даты</th>
                    <th className="text-center">Сумма и Статус</th>
                    <th className="text-center">Контакты</th>
                </tr>
            </thead>
            <tbody>{renderInvoices}</tbody>
        </table>
    );
};

export default InvoicesListComponent;