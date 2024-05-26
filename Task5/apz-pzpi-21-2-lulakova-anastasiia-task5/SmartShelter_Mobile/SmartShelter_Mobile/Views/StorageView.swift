//
//  StorageView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 26.05.2024.
//

import SwiftUI

struct StorageView: View {
    @ObservedObject public var storageVM = StorageVM()
    
    
    var body: some View {
       // ScrollView{
            VStack{
                storageTable
                Divider()
                orderTable
            }
        //}
        .onAppear{
            storageVM.getFullStorage()
            storageVM.getAllOrders()
        }
    }
    
    var storageTable:some View{
        VStack{
            Text("Full Storage")
                .font(.largeTitle)
                .padding()
            tableHeader
            List(storageVM.storage) { storage in
                tableRow(storage: storage)
            }
            .listStyle(PlainListStyle())
            .padding(.vertical, 0)
        }
    }
    
    var orderTable:some View{
        VStack(spacing: 0) {
            Text("Order Table")
                .font(.largeTitle)
                .padding()
            orderTableHeader
            List(storageVM.orders) { order in
                orderTableRow(order: order)
            }
            .listStyle(PlainListStyle())
            .padding(.vertical, 0)
        }
    }
    
    var tableHeader: some View {
            HStack {
                Text("Name")
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text("Amount")
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text("UOM")
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text("Price")
                    .frame(maxWidth: .infinity, alignment: .leading)
            }
            .padding()
            .background(Color.gray.opacity(0.2))
        }

        func tableRow(storage: Storage) -> some View {
            HStack {
                Text(storage.name)
                   .frame(maxWidth: .infinity, alignment: .leading)
                Text("\(storage.amount, specifier: "%.2f")")
                   .frame(maxWidth: .infinity, alignment: .leading)
                Text(storage.unitOfMeasure)
                   .frame(maxWidth: .infinity, alignment: .leading)
                Text("\(storage.price, specifier: "%.2f")")
                    .frame(maxWidth: .infinity, alignment: .leading)
            }
            //.padding()
        }
    
    
    var orderTableHeader: some View {
            HStack {
                Text("Name")
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text("Amount")
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text("Price")
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text("Order Date")
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text("Actions")
                    .frame(maxWidth: .infinity, alignment: .leading)
            }
            .padding(.horizontal)
            .padding(.vertical, 4)
            .background(Color.gray.opacity(0.2))
        }

        func orderTableRow(order: Order) -> some View {
            HStack {
                Text(order.name)
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text("\(order.amount, specifier: "%.2f")")
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text("\(order.price, specifier: "%.2f")")
                    .frame(maxWidth: .infinity, alignment: .leading)
                Text(order.orderDate)
                    .frame(maxWidth: .infinity, alignment: .leading)
                if !order.isApproved {
                    Button(action: {
                        storageVM.approveOrder(order: order)
                    }) {
                        Text("Approve")
                            .padding(.horizontal)
                            .padding(.vertical, 4)
                            .background(Color.blue)
                            .foregroundColor(.white)
                            .cornerRadius(5)
                    }
                } else {
                    Text("Approved")
                        .frame(maxWidth: .infinity, alignment: .leading)
                        .foregroundColor(.green)
                }
            }
            .padding(.horizontal)
            .padding(.vertical, 4)
        }
}

#Preview {
    StorageView()
}
