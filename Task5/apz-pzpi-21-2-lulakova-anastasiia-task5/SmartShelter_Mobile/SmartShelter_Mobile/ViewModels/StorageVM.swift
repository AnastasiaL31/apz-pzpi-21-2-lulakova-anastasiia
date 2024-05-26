//
//  StorageVM.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 26.05.2024.
//

import Foundation


class StorageVM: ObservableObject {
    @Published public var storage:Array<Storage> = []
    @Published public var orders:Array<Order> = []
    
    public func getFullStorage(){
        Storage.getFullStorage{ result in
            switch result{
            case .success(var storage):
                DispatchQueue.main.async {
                    for number in 0..<storage.count {
                        storage[number].date = DateConverter.fromServerDateToString(dateString: storage[number].date, time: .omitted)
                    }
                    print(storage)
                    self.storage = storage
                }
            case .failure(let error):
                print(error)
            }
        }
    }
    
    public func getAllOrders(){
        Order.getAllOrders { result in
            switch result{
            case .success(var orders):
                DispatchQueue.main.async {
                    for number in 0..<orders.count {
                        orders[number].orderDate = DateConverter.fromServerDateToString(dateString: orders[number].orderDate, time: .omitted)
                    }
                    print(orders)
                    self.orders = orders
                }
            case .failure(let error):
                print(error)
            }
        }
    }
    
    public func approveOrder(order:Order){
        order.approveOrder{ result in
            if(result){
                DispatchQueue.main.async {
                    self.storage.append(Storage(order: order))
                    self.storage = Storage.groupStorage(self.storage)
                    if let index = self.orders.firstIndex(where: {$0.id == order.id}){
                        self.orders[index].isApproved = true
                    }
                }
            }
        }
    }
}
